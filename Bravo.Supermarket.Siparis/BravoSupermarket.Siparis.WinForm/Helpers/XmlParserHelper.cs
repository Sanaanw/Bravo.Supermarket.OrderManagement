using BravoSupermarket.Siparis.WinForm.Dtos;
using System;
using System.Collections.Generic;
using System.Xml;

namespace BravoSupermarket.Siparis.WinForm.Helpers
{
    public class XmlParserHelper
    {
        public static OrderDto ParseXml(string filePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);

            // Header
            var headerNode = xmlDoc.SelectSingleNode("//Order-Header");
            if (headerNode == null)
                throw new Exception("Order-Header tapılmadı.");

            var header = new OrderHeaderDto
            {
                OrderNumber = headerNode["OrderNumber"]?.InnerText,
                OrderDate = headerNode["OrderDate"]?.InnerText,
                OrderCurrency = headerNode["OrderCurrency"]?.InnerText,
                ExpectedDeliveryDate = headerNode["ExpectedDeliveryDate"]?.InnerText,
                AdditionalRemarks = headerNode["AdditionalRemarks"]?.InnerText,
                TestInformation = headerNode["TestInformation"]?.InnerText,
                ContractNumber = headerNode["ContractNumber"]?.InnerText,
                PaymentTerms = headerNode.SelectSingleNode("Payment/PaymentTerms/Terms")?.InnerText
            };

            // Parties
            var parties = new OrderPartyDto
            {
                BuyerILN = xmlDoc.SelectSingleNode("//Order-Parties/Buyer/ILN")?.InnerText,
                SellerILN = xmlDoc.SelectSingleNode("//Order-Parties/Seller/ILN")?.InnerText,
                CodeByBuyer = xmlDoc.SelectSingleNode("//Order-Parties/Seller/CodeByBuyer")?.InnerText,
                DeliveryPointILN = xmlDoc.SelectSingleNode("//Order-Parties/DeliveryPoint/ILN")?.InnerText,
                DeliveryPointName = xmlDoc.SelectSingleNode("//Order-Parties/DeliveryPoint/Name")?.InnerText,
            };

            // Lines
            var lines = new List<OrderLineDto>();
            var lineNodes = xmlDoc.SelectNodes("//Order-Lines/Line");
            foreach (XmlNode line in lineNodes)
            {
                var item = line.SelectSingleNode("Line-Item");
                if (item == null)
                    continue;

                lines.Add(new OrderLineDto
                {
                    LineNumber = int.TryParse(item["LineNumber"]?.InnerText, out int ln) ? ln : 0,
                    EAN = item["EAN"]?.InnerText,
                    BuyerItemCode = item["BuyerItemCode"]?.InnerText,
                    ItemDescription = item["ItemDescription"]?.InnerText,
                    ItemType = item["ItemType"]?.InnerText,
                    OrderedQuantity = double.TryParse(item["OrderedQuantity"]?.InnerText, out double oq) ? oq : 0,
                    FreeOrderedQuantity = double.TryParse(item["FreeOrderedQuantity"]?.InnerText, out double fq) ? fq : 0,
                    OrderedUnitPacksize = double.TryParse(item["OrderedUnitPacksize"]?.InnerText, out double ps) ? ps : 0,
                    UnitOfMeasure = item["UnitOfMeasure"]?.InnerText,
                    OrderedUnitNetPrice = double.TryParse(item["OrderedUnitNetPrice"]?.InnerText, out double pr) ? pr : 0,
                    ScheduledQuantity = int.TryParse(item.SelectSingleNode("ScheduleQuantities/ScheduleQuantity/Quantity")?.InnerText, out int sq) ? sq : 0,
                });
            }

            // Summary
            int totalLines = int.TryParse(xmlDoc.SelectSingleNode("//Order-Summary/TotalLines")?.InnerText, out int tl) ? tl : 0;

            return new OrderDto
            {
                Header = header,
                Parties = parties,
                Lines = lines,
                TotalLines = totalLines
            };
        }
    }
}
