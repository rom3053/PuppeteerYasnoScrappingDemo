namespace WebScrappingDemo.Common.Dtos;

public class FileDto
{
    public string Name { get; set; } = string.Empty;

    public byte[] Bytes { get; set; }

    public string Extension { get; set; } = string.Empty;

    public string MediaType { get; set; } = string.Empty;
}