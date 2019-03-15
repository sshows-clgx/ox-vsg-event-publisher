import { Component, OnInit, ViewChild } from '@angular/core';
import { DatatableComponent } from '@swimlane/ngx-datatable';
import { AdminService } from 'src/app/services/admin.service';
import { Configuration } from 'src/app/models/configuration';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  @ViewChild('configurationsTable') configurationsTable: DatatableComponent;
  public loading: boolean = true;
  public configurations: Configuration[] = [];
  public editModeItems: any = {};
  public expanded: any = {};

  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.adminService.getConfigurations()
      .subscribe(res => {
        this.configurations = res;
        this.loading = false;
      });
  }

  toggleEditMode(index: number, field: string) {
    this.editModeItems[index + '-' + field] = !this.editModeItems[index + '-' + field];
  }

  toggleExpandRow(row) {
    console.log('Toggled Expand Row!', row);
    this.configurationsTable.rowDetail.toggleExpandRow(row);
  }

}
