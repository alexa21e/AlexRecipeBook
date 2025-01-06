import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { SharedModule } from '../shared/shared.module';
import { HomeComponent } from './home/home.component';
import { RecipeComponent } from './recipe/recipe.component';
import { AuthorComponent } from './author/author.component';

import { DataViewModule } from 'primeng/dataview';
import { TableModule } from 'primeng/table';
import { DividerModule } from 'primeng/divider';
import { CardModule } from 'primeng/card';
import { FieldsetModule } from 'primeng/fieldset';
import { InputTextModule } from 'primeng/inputtext';
import { ListboxModule } from 'primeng/listbox';
import { ButtonModule } from 'primeng/button';
import { ProgressSpinnerModule } from 'primeng/progressspinner';

@NgModule({
  declarations: [
    HomeComponent,
    RecipeComponent,
    AuthorComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule,
    FormsModule,
    DataViewModule,
    TableModule,
    DividerModule,
    CardModule,
    FieldsetModule,
    InputTextModule,
    FormsModule,
    ListboxModule,
    ButtonModule,
    ProgressSpinnerModule
  ],
  exports: [
    HomeComponent
  ]
})
export class FeaturesModule { }
