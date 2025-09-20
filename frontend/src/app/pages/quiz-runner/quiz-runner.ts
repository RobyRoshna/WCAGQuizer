import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';
import { QuizQuestion, QuizResult, QuizSubmitPayload } from '../../models';

@Component({
  standalone: true,
  selector: 'app-quiz-runner',
  templateUrl: './quiz-runner.html',
  styleUrls: ['./quiz-runner.css'],
  imports: [FormsModule]
})
export class QuizRunner implements OnInit {
  criterionId = 0;
  questions: QuizQuestion[] = [];
  answers: Record<number,{single?:number; multi:Set<number>; fill?:string}> = {};
  result?: QuizResult;

  constructor(private api: ApiService, private route: ActivatedRoute) {}

  ngOnInit(){
    this.criterionId = Number(this.route.snapshot.paramMap.get('id'));
    this.api.getQuiz(this.criterionId).subscribe(qs => {
      this.questions = qs;
      for (const q of qs) this.answers[q.id] = { multi:new Set<number>() };
    });
  }
  toggle(qid:number, oid:number, checked:boolean){
    const set = this.answers[qid].multi;
    checked ? set.add(oid) : set.delete(oid);
  }
  submit() {
    const payload = {
      criterionId: this.criterionId,
      answers: this.questions.map(q => {
        
        let selectedOptionIds: number[] = [];
        if (q.type === 'single') {
          const single = this.answers[q.id].single;
          selectedOptionIds = single != null ? [single] : []; // never [undefined]
        } else if (q.type === 'multi') {
          selectedOptionIds = Array.from(this.answers[q.id].multi); // Set<number> -> number[]
        } // fill -> []

        // only include textAns for fill; otherwise leave it undefined
        const textAns = q.type === 'fill' ? (this.answers[q.id].fill || '') : undefined;

        return {
          questionId: q.id,
          selectedOptionIds,
          textAns
        };
      })
    } satisfies QuizSubmitPayload; 

    this.api.submitQuiz(payload).subscribe(res => this.result = res);
  }

}

