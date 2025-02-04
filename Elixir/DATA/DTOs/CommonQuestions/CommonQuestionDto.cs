using System;

namespace Elixir.DATA.DTOs.CommonQuestions;

public class CommonQuestionDto : BaseDto<Guid>
{
    public string? Title { get; set; }
    public string? Description { get; set; }
}
