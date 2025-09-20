import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment.development';
import { CriterionListItem, CriterionDetail, QuizQuestion, QuizSubmitPayload, QuizResult } from '../models';

@Injectable({ providedIn: 'root' })
export class ApiService {
  private baseUrl = 'http://localhost:5036/api';
  private http = inject(HttpClient);

  getCriteria() {
    return this.http.get<CriterionListItem[]>(`${this.baseUrl}/criteria`);
  }
  getCriterion(id: number) {
    return this.http.get<CriterionDetail>(`${this.baseUrl}/criteria/${id}`);
  }
  getQuiz(criterionId: number) {
    return this.http.get<QuizQuestion[]>(`${this.baseUrl}/criteria/${criterionId}/quiz`);
  }
  submitQuiz(payload: QuizSubmitPayload) {
    return this.http.post<QuizResult>(`${this.baseUrl}/quiz/submit`, payload);
  }
}
