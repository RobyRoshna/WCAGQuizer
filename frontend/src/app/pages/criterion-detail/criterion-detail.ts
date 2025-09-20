import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { CriterionDetail } from '../../models';

@Component({
  standalone: true,
  selector: 'app-criterion-detail',
  templateUrl: './criterion-detail.html',
  styleUrls: ['./criterion-detail.css'],
  imports: [RouterLink]
})
export class CriterionDetailComponent implements OnInit {
  c?: CriterionDetail;
  constructor(private api: ApiService, private route: ActivatedRoute) {}
  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.api.getCriterion(id).subscribe(d => this.c = d);
  }
}
