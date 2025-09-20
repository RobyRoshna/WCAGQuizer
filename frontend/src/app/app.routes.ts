import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', loadComponent: () => import('./pages/home/home').then(m => m.Home) },
  { path: 'criteria', loadComponent: () => import('./pages/criteria-list/criteria-list').then(m => m.CriteriaList) },
  { path: 'criteria/:id', loadComponent: () => import('./pages/criterion-detail/criterion-detail').then(m => m.CriterionDetailComponent) },
  { path: 'quiz/:id', loadComponent: () => import('./pages/quiz-runner/quiz-runner').then(m => m.QuizRunner) },
  { path: '**', redirectTo: '' }
];
