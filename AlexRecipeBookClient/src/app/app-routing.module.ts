import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { RecipeComponent } from './features/recipe/recipe.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {path: 'recipe/:name', component: RecipeComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
