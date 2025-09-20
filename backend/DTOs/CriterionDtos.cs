namespace WCAGQuizer.DTOs;

public record CriterionListItemDto(
	int Id, string Code, string Name, string Level, string? Description);

public record BestPracticeDto(
	int Id, string Title, string Kind, string Body);

public record CodeSnippetDto(
	int Id, string Language, bool IsAntiPattern, string Content);

public record CriterionDetailDto(
	int Id, string Code, string Name, string Level, string? Description,
	List<BestPracticeDto> BestPractices,
	List<CodeSnippetDto> CodeSnippets);
