using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using SWIFTCARGOAPI.Models;
using System.Collections.Generic;
using System.Linq;

namespace SWIFTCARGOAPI.Documents
{
    public class ShipmentReport : IDocument
    {
        private readonly IEnumerable<Shipment> _shipments;
        private readonly string _title;

        public ShipmentReport(IEnumerable<Shipment> shipments, string title)
        {
            _shipments = shipments;
            _title = title;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container
                .Page(page =>
                {
                    page.Margin(50);

                    page.Header().Element(ComposeHeader);
                    page.Content().Element(ComposeContent);
                    page.Footer().AlignCenter().Text(x =>
                    {
                        x.CurrentPageNumber();
                        x.Span(" / ");
                        x.TotalPages();
                    });
                });
        }

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text(_title)
                        .FontSize(20).SemiBold().FontColor(Colors.Blue.Medium);

                    column.Item().Text($"Report generated on: {System.DateTime.Now:yyyy-MM-dd HH:mm}")
                        .FontSize(9);
                });
            });
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Column(column =>
            {
                column.Spacing(20);
                column.Item().Element(ComposeTable);
            });
        }

        void ComposeTable(IContainer container)
        {
            container.Table(table =>
            {
                // Define columns
                table.ColumnsDefinition(columns =>
                {
                    columns.ConstantColumn(25);
                    columns.RelativeColumn(3);
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                // Define header
                table.Header(header =>
                {
                    header.Cell().Text("#");
                    header.Cell().Text("Tracking #").Bold();
                    header.Cell().Text("Origin").Bold();
                    header.Cell().Text("Destination").Bold();
                    header.Cell().Text("Status").Bold();
                    header.Cell().Text("Est. Delivery").Bold();
                });

                // Add rows
                foreach (var (shipment, index) in _shipments.Select((value, i) => (value, i)))
                {
                    table.Cell().Text((index + 1).ToString());
                    table.Cell().Text(shipment.TrackingNumber);
                    table.Cell().Text(shipment.Origin);
                    table.Cell().Text(shipment.Destination);
                    table.Cell().Text(shipment.Status);
                    table.Cell().Text(shipment.EstimatedDelivery.ToString("yyyy-MM-dd"));
                }
            });
        }
    }
}