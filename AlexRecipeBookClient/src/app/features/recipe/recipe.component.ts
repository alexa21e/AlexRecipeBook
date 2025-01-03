import { Component, OnInit } from '@angular/core';
import { RecipesService } from '../../shared/services/recipes.service';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { DetailedRecipe } from '../../shared/models/detailedRecipe';
import { SimilarRecipe } from '../../shared/models/similarRecipe';

@Component({
  selector: 'app-recipe',
  templateUrl: './recipe.component.html',
  styleUrl: './recipe.component.css'
})
export class RecipeComponent implements OnInit {
  private recipeSubscription: Subscription = new Subscription();
  private similarRecipesSubscription: Subscription = new Subscription();

  recipeDetails!: DetailedRecipe;
  similarRecipes: SimilarRecipe[] = [];

  isLoading = true;

  constructor(private recipesService: RecipesService,
    private activatedRoute: ActivatedRoute,
    private router: Router) {
      this.router.events.subscribe((event) => {
        if (event instanceof NavigationEnd) {
         this.loadData();
        }
    });
  }

  ngOnInit(): void {
    this.loadData();
    console.log(this.isLoading);
  }

  getRecipe(id: string) {
    this.recipeSubscription = this.recipesService.getRecipe(id).subscribe(
      {
        next: (response) => {
          this.recipeDetails = response;
          this.isLoading = false;
        },
        error: error => {
          console.log(error);
          this.isLoading = false;
        }
      }
    );
  }

  getFiveMostSimilarRecipes(id: string) {
    this.similarRecipesSubscription = this.recipesService.getFiveMostSimilarRecipes(id).subscribe(
      {
        next: (response) => {
          this.similarRecipes = response;
          this.isLoading = false;
        },
        error: error => {
          console.log(error);
          this.isLoading = false;
        }
      }
    );
  }

  loadData() {
    this.activatedRoute.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.getRecipe(id);
        this.getFiveMostSimilarRecipes(id);
      }
    });
  }

  onSimilarRecipeClick(id: string) {
    this.router.navigate(['/recipe', id]);
  }

  ngOnDestroy(): void {
    this.recipeSubscription.unsubscribe();
    this.similarRecipesSubscription.unsubscribe();
  }
}