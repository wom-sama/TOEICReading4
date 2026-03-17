using System.ComponentModel.DataAnnotations;

namespace TOEICReading4.Users.Dto;

public class ChangeUserLanguageDto
{
    [Required]
    public string LanguageName { get; set; }
}