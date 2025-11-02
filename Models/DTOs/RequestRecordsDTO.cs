using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BikeBuster.Models.DTOs;

public record LicenseUploadRequest([property: JsonPropertyName("imagem_cnh")] string ImageBase64);
public record RentalReturnRequest([property: JsonPropertyName("data_devolucao")] DateTime ReturnDate);
public record PlateUpdateRequest([property: JsonPropertyName("placa")] string NewPlate);

