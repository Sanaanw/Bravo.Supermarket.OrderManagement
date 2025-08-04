using Bravo.Supermarket.API.Data;
using Bravo.Supermarket.API.Dto.ToSql;
using Bravo.Supermarket.API.Dto.Xml;
using Bravo.Supermarket.API.Parametrs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Xml;

namespace Bravo.Supermarket.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OperationController : ControllerBase
    {
        private readonly sql_operation _sql;
        private readonly AppDbContext _context;
        //sql_operation sql;
        string _DB_Name = "";
        string _Old_DB_Name = ""; //Old Db Name 
        string _First_DB_Name = "";

        public OperationController(AppDbContext context, sql_operation sql)
        {
            _context = context;
            _sql = sql;
            _DB_Name = "MikroDB_V15_ARAN_2024";
        }

        [HttpPost("ReceiveOrder")]
        public IActionResult ReceiveOrder([FromBody] OrderDto orderDto,string mesmer_code)
        {
            ERP_CONFIG();

            string connStr = _context.Database.GetDbConnection().ConnectionString;


            var listpar = new List<Parameters>();
            string _query = "SELECT TOP 1 * FROM  " + _DB_Name + "..SIPARISLER";
            DataTable dt = _sql.dbRun(listpar, _query);

            string ficheno_new_seri = "TMOR01";
            DataTable dt_sira = _sql.dbRun(listpar, "select isnull(MAX(isnull(sip_evrakno_sira,0)),0)+1 from " + _DB_Name + "..SIPARISLER	WHERE sip_evrakno_seri='" + ficheno_new_seri + "' ");
            double ficheno_new_sira = Convert.ToDouble(dt_sira.Rows[0][0]);


            //
            //string fichenoNewSiraQuery = $"SELECT ISNULL(MAX(sip_evrakno_sira), 0) + 1 FROM  " + _DB_Name + "..SIPARISLER";
            //DataTable dtFichenoSiraIds = _sql.dbRun(new List<Parameters>(), fichenoNewSiraQuery);
            //int ficheno_new_sira = Convert.ToInt32(dtFichenoSiraIds.Rows[0][1]);




            string sip_projekod = "998";
            string sip_mesmercode = string.IsNullOrEmpty(mesmer_code) ? "" : mesmer_code;

            using (SqlConnection con_logo = new SqlConnection(connStr))
            {
                con_logo.Open();
                using (SqlTransaction transaction = con_logo.BeginTransaction())
                {
                    try
                    {
                        int linenr = 0;
                        foreach (var line in orderDto.Lines)
                        {
                            var gettingBarkodTanimlar = _context.barkodTanimlari.FirstOrDefault(x => x.bar_kodu == line.EAN);
                            if (gettingBarkodTanimlar == null)
                                return BadRequest("Barkod tapılmadı.");

                            var gettingCariHesablar = _context.CARI_HESAPLAR
                                .FirstOrDefault(x => x.cari_unvan1 != null && x.cari_unvan1.Contains("2027") && x.cari_unvan1.Contains("Bravo"));

                            if (gettingCariHesablar == null)
                                return BadRequest("Cari Hesab tapılmadı.");

                            var gettingCariHesapAdresleri = _context.CARI_HESAP_ADRESLERI
                                .FirstOrDefault(x => x.adr_cari_kod == gettingCariHesablar.cari_kod);

                            if (gettingCariHesapAdresleri == null)
                                return BadRequest("Cari Hesap Adresləri tapılmadı.");

                            SendingOrderDto sendingOrderDto = new SendingOrderDto
                            {
                                sip_stok_kod = gettingBarkodTanimlar.bar_stokkodu,
                                sip_miktar = line.OrderedQuantity,
                                sip_satici_kod = gettingCariHesapAdresleri.adr_temsilci_kodu,
                                sip_b_fiyat = line.OrderedUnitNetPrice,
                                sip_tutar = line.OrderedUnitNetPrice * line.OrderedQuantity,
                                sip_iskonto1 = (line.OrderedUnitNetPrice * line.OrderedQuantity) / 4,
                                sip_depono = 2,
                                sip_aciklama = orderDto.Header.OrderNumber,
                                sip_musteri_kod = gettingCariHesablar.cari_kod
                            };

                            using (SqlCommand command = con_logo.CreateCommand())
                            {
                                command.Transaction = transaction;
                                linenr++;
                                InsertOrderToDatabase(
                                    sendingOrderDto,
                                    sip_projekod,
                                    sip_mesmercode,
                                    ficheno_new_seri,
                                    ficheno_new_sira,
                                    linenr, 
                                    odemePlaniNo: -14,
                                    command);
                            }
                        }

                        transaction.Commit();
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return StatusCode(500, $"Xəta baş verdi: {ex.Message}");
                    }
                }
            }
        }

        private void InsertOrderToDatabase(
            SendingOrderDto sendingOrderDto,
            string sip_projekodu,
            string sip_mesmercode,
            string ficheno_new_seri,
            double ficheno_new_sira,
            int linenr,
            int odemePlaniNo,
            SqlCommand command)
        {

            string query_line = "INSERT INTO " + _DB_Name + "..SIPARISLER  ( sip_RECid_DBCno,sip_RECid_RECno,sip_SpecRECno,sip_iptal,sip_fileid,sip_hidden,sip_kilitli,sip_degisti,sip_checksum,sip_create_user,sip_create_date,sip_lastup_user,sip_lastup_date,sip_special1,sip_special2,sip_special3,sip_firmano,sip_subeno,sip_tarih,sip_teslim_tarih,sip_tip,sip_cins,sip_evrakno_seri,sip_evrakno_sira,sip_satirno,sip_belgeno,sip_belge_tarih,sip_satici_kod,sip_musteri_kod,sip_stok_kod,sip_b_fiyat,sip_miktar,sip_birim_pntr,sip_teslim_miktar,sip_tutar,sip_iskonto_1,sip_iskonto_2,sip_iskonto_3,sip_iskonto_4,sip_iskonto_5,sip_iskonto_6,sip_masraf_1,sip_masraf_2,sip_masraf_3,sip_masraf_4,sip_vergi_pntr,sip_vergi,sip_masvergi_pntr,sip_masvergi,sip_opno,sip_aciklama,sip_aciklama2,sip_depono,sip_OnaylayanKulNo,sip_vergisiz_fl,sip_kapat_fl,sip_promosyon_fl,sip_cari_sormerk,sip_stok_sormerk,sip_cari_grupno,sip_doviz_cinsi,sip_doviz_kuru,sip_alt_doviz_kuru,sip_adresno,sip_teslimturu,sip_cagrilabilir_fl,sip_prosiprecDbId,sip_prosiprecrecI,sip_iskonto1,sip_iskonto2,sip_iskonto3,sip_iskonto4,sip_iskonto5,sip_iskonto6,sip_masraf1,sip_masraf2,sip_masraf3,sip_masraf4,sip_isk1,sip_isk2,sip_isk3,sip_isk4,sip_isk5,sip_isk6,sip_mas1,sip_mas2,sip_mas3,sip_mas4,sip_Exp_Imp_Kodu,sip_kar_orani,sip_durumu,sip_stalRecId_DBCno,sip_stalRecId_RECno,sip_planlananmiktar,sip_teklifRecId_DBCno,sip_teklifRecId_RECno,sip_parti_kodu,sip_lot_no,sip_projekodu,sip_fiyat_liste_no,sip_Otv_Pntr,sip_Otv_Vergi,sip_otvtutari,sip_OtvVergisiz_Fl,sip_paket_kod,sip_RezRecId_DBCno,sip_RezRecId_RECno,sip_harekettipi,sip_yetkili_recid_dbcno,sip_yetkili_recid_recno,sip_kapatmanedenkod,sip_gecerlilik_tarihi,sip_onodeme_evrak_tip,sip_onodeme_evrak_seri,sip_onodeme_evrak_sira,sip_rezervasyon_miktari,sip_rezerveden_teslim_edilen) VALUES(0,0,0,0,21,0,0,0,0,1,getdate(),1,getdate(),N'',N'',N'',0,0,(select convert(varchar(10),GETDATE(),120)),(select convert(varchar(10),GETDATE(),120)),0,0,N'" + ficheno_new_seri + "'," + ficheno_new_sira + "," + linenr + ",N'',(select convert(varchar(10),GETDATE(),120)),N'" + sendingOrderDto.sip_satici_kod + "',N'" + sendingOrderDto.sip_musteri_kod + "',N'" + sendingOrderDto.sip_stok_kod + "'," + sendingOrderDto.sip_b_fiyat + "," + sendingOrderDto.sip_miktar + ",1,0," + sendingOrderDto.sip_tutar + "," + sendingOrderDto.sip_iskonto1 + "," + "0" + "," + "0" + "," + "0" + "," + "0" + "," + "0" + ",0,0,0,0,1,0,0,0,'" + odemePlaniNo + "',N'" + sendingOrderDto.sip_aciklama + "',N'','" + sendingOrderDto.sip_depono + "',0,0,0,0,N'" + sip_mesmercode + "',N'" + sip_mesmercode + "',0,0,1.000000000000,1.000000000000,0,N'',1,0,0,   0,1,1,1,1,1,1,1,1,1 ,0,0,0,0,0,0,0,0,0,0,N'',0,0,0,0,0,0,0,N'',0,N'" + sip_projekodu + "',0,0,0,0,0,N'',0,0,0,0,0,N'',(select convert(varchar(10),GETDATE(),120)),0,N'',0,0,0)";


           

            command.Parameters.AddWithValue("@ficheno_new_seri", ficheno_new_seri);
            command.Parameters.AddWithValue("@ficheno_new_sira", ficheno_new_sira);
            command.Parameters.AddWithValue("@linenr", linenr);
            command.Parameters.AddWithValue("@sip_satici_kod", sendingOrderDto.sip_satici_kod ?? "");
            command.Parameters.AddWithValue("@sip_musteri_kod", sendingOrderDto.sip_musteri_kod ?? "");
            command.Parameters.AddWithValue("@sip_stok_kod", sendingOrderDto.sip_stok_kod ?? "");
            command.Parameters.AddWithValue("@sip_b_fiyat", sendingOrderDto.sip_b_fiyat);
            command.Parameters.AddWithValue("@sip_miktar", sendingOrderDto.sip_miktar);
            command.Parameters.AddWithValue("@sip_tutar", sendingOrderDto.sip_tutar);
            command.Parameters.AddWithValue("@sip_iskonto1", sendingOrderDto.sip_iskonto1);
            command.Parameters.AddWithValue("@odemePlaniNo", odemePlaniNo);
            command.Parameters.AddWithValue("@sip_aciklama", sendingOrderDto.sip_aciklama ?? "");
            command.Parameters.AddWithValue("@sip_depono", sendingOrderDto.sip_depono);
            command.Parameters.AddWithValue("@sip_mesmercode", sip_mesmercode ?? "");

            command.CommandText = query_line;
            var sqlreader = command.ExecuteReader();
            sqlreader.Close();

            // Update əməliyyatı
            string query_rec = "UPDATE " + _DB_Name + "..SIPARISLER SET sip_RECid_RECno = sip_RECno where sip_tip = 0 and sip_evrakno_sira = '" + ficheno_new_sira + "' and sip_evrakno_seri = N'" + ficheno_new_seri + "'";

            command.Parameters.Clear();
            command.CommandText = query_rec;
            sqlreader = command.ExecuteReader();
            sqlreader.Close();
            //command.CommandText = query_rec;
            //var sqlreader = command.ExecuteReader();
            //sqlreader.Close();


        }


        private void ERP_CONFIG()
        {
            string _query = "SELECT TOP 1 * FROM TN_MIKRO_DB_INFO";
            List<Parameters> list_par = new List<Parameters>();
            DataTable dt_ = _sql.dbRun(list_par, _query);
            if (dt_.Rows.Count > 0)
            {
                _DB_Name = dt_.Rows[0]["DB_NAME"].ToString();
                _Old_DB_Name = dt_.Rows[0]["OLD_DB_NAME"].ToString();
                _First_DB_Name = dt_.Rows[0]["FIRST_DB_NAME"].ToString();
            }
        }
    }
}