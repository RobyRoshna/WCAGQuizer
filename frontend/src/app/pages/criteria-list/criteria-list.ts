import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ApiService } from '../../services/api.service';
import { CriterionListItem } from '../../models';

@Component({
  standalone: true,
  selector: 'app-criteria-list',
  templateUrl: './criteria-list.html',
  styleUrls: ['./criteria-list.css'],
  imports: [RouterLink]
})
export class CriteriaList implements OnInit {
  criteria: CriterionListItem[] = [];
  constructor(private api: ApiService) {}
  ngOnInit() { this.api.getCriteria().subscribe(d => this.criteria = d); }
}
