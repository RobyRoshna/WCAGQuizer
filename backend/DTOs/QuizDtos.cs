
namespace WCAGQuizer.DTOs;

public record QuizAnswerOptionDto(int Id, string Text);

public record QuizQuestionDto(
	int Id, string Text, string Type, string? Explanation,
	List<QuizAnswerOptionDto> Options);

// Result for POST /api/quiz/submit
public record QuizFeedbackDto(
	int QuestionId,
	List<int> SelectedOptionIds,
	List<int> CorrectOptionIds,
	bool IsCorrect,
	string? Explanation
);

public record QuizResultDto(
	int CriterionId,
	int TotalQuestions,
	int Correct,
	List<QuizFeedbackDto> Details
);
