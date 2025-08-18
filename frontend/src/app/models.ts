//For DTOs
export interface CriterionListItem {
  id: number;
  code: string;
  name: string;
  level: string;
  description: string;
}

export interface BestPractice {
  id: number;
  kind: 'best-practice' | 'pitfall';
  title: string;
  body: string;
}

export interface CodeSnippet {
  id: number;
  language: string;
  isAntiPattern: boolean;
  content: string;
}

export interface CriterionDetail extends CriterionListItem {
  bestPractices: BestPractice[];
  codeSnippets: CodeSnippet[];
}

export type QuestionType = 'single' | 'multi' | 'fill';

export interface QuizOption {
  id: number;
  text: string;
  isCorrect?: boolean; // not sent by API in real life
}

export interface QuizQuestion {
  id: number;
  criterionId: number;
  type: QuestionType;
  text: string;
  explanation?: string;
  options?: QuizOption[];
}

export interface QuizSubmitAnswer {
  questionId: number;
  selectedOptionIds?: number[];
  freeText?: string | null;
}

export interface QuizSubmitPayload {
  criterionId: number;
  answers: QuizSubmitAnswer[];
}

export interface QuizResult {
  total: number;
  score: number;
  feedback: { questionId: number; correct: boolean; explanation?: string }[];
}
