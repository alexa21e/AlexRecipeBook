import { Component, OnInit } from '@angular/core';
import { RecipesService } from '../../shared/services/recipes.service';
import { RecipeParams } from '../../shared/models/recipeParams';
import { Router } from '@angular/router';
import { SortEvent } from 'primeng/api';
import { ListboxFilterEvent } from 'primeng/listbox';
import { HomeRecipe } from '../../shared/models/homeRecipe';
import { debounceTime, distinctUntilChanged, Subject, Subscription, switchMap } from 'rxjs';
import { Ingredient } from '../../shared/models/ingredient';
import { Author } from '../../shared/models/author';
import { RecipeStats } from '../../shared/models/recipeStats';
import { IngredientParams } from '../../shared/models/ingredientParams';
import { IngredientsService } from '../../shared/services/ingredients.service';
import { AuthorsService } from '../../shared/services/authors.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})

export class HomeComponent implements OnInit {
  private recipesSubscription: Subscription = new Subscription();
  private ingredientsSubscription: Subscription = new Subscription();
  private mostUsedIngredientsSubscription: Subscription = new Subscription();
  private mostProlificAuthorsSubscription: Subscription = new Subscription();
  private mostComplexRecipesSubscription: Subscription = new Subscription();

  private ingredientSearchTerm = new Subject<string>();
  private recipeSearchTerm = new Subject<string>();
  searchText: string = '';

  recipes: HomeRecipe[] = [];
  ingredients: Ingredient[] = [];
  commonIngredients: Ingredient[] = [];
  prolificAuthors: Author[] = [];
  complexRecipes: RecipeStats[] = [];

  recipeParams = new RecipeParams();
  ingredientsParams = new IngredientParams();

  recipesCount: number = 0;
  rowsPerPageOptions: number[] = [20];

  selectedIngredients!: any[];
  selectAll: any;

  selectedSortOption: string = '_asc';

  isLoading = true;

  constructor(
    private recipesService: RecipesService,
    private ingredientsService: IngredientsService,
    private authorsService: AuthorsService,
    private router: Router) {
  }

  ngOnInit(): void {
    this.getRecipes();
    this.getIngredients();
    this.ingredientSearchTerm.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((term: string) => this.ingredientsService.getIngredients({ name: term, ingredientsDisplayedNo: 200 }))
    ).subscribe(ingredients => this.ingredients = ingredients.data);
    this.recipeSearchTerm.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      switchMap((term: string) => {
        this.searchText = term;
        return this.recipesService.getRecipes(this.recipeParams);
      })
    ).subscribe(
      {
        next: (response) => {
          this.recipes = response.data;
          this.recipeParams.pageNumber = response.pageNumber;
          this.recipeParams.pageSize = response.pageSize;
          this.recipesCount = response.count;
          this.isLoading = false;
        },
        error: error => console.log(error)
      }
    );
    this.getStats();
  }

  getRecipes() {
    this.recipeParams.sortOrder = this.selectedSortOption;
    if (this.searchText) {
      this.recipeParams.recipeName = this.searchText;
    }
    this.recipesSubscription = this.recipesService.getRecipes(this.recipeParams).subscribe(
      {
        next: (response) => {
          this.recipes = response.data;
          this.recipeParams.pageNumber = response.pageNumber;
          this.recipeParams.pageSize = response.pageSize;
          this.recipesCount = response.count;
          this.isLoading = false;
        },
        error: error => console.log(error)
      }
    );
  }

  getIngredients() {
    this.ingredientsSubscription = this.ingredientsService.getIngredients(this.ingredientsParams).subscribe(
      {
        next: (response) => {
          this.ingredients = response.data;
          this.ingredientsParams.ingredientsDisplayedNo = response.ingredientsDisplayedNo;
        },
        error: error => console.log(error)
      }
    );
  }

  getStats() {
    this.mostUsedIngredientsSubscription = this.ingredientsService.getMostCommonIngredients(5).subscribe(
      {
        next: (response) => {
          this.commonIngredients = response;
        },
        error: error => console.log(error)
      }
    );
    this.mostProlificAuthorsSubscription = this.authorsService.getMostProlificAuthors(5).subscribe(
      {
        next: (response) => {
          this.prolificAuthors = response;
        },
        error: error => console.log(error)
      }
    );
    this.mostComplexRecipesSubscription = this.recipesService.getMostComplexRecipes(5).subscribe(
      {
        next: (response) => {
          this.complexRecipes = response;
          console.log(this.complexRecipes);
        },
        error: error => console.log(error)
      }
    );
  }

  onPageChange(event: any) {
    const newPageNumber = event.first / event.rows + 1;
    if (this.recipeParams.pageNumber !== newPageNumber) {
      this.recipeParams.pageNumber = newPageNumber;
      this.getRecipes();
    }
  }

  onRecipeClick(recipeId: string) {
    this.router.navigate(['/recipe', recipeId]);
  }

  onRecipeHover(recipe: HomeRecipe) {
    this.router.navigate(['/recipe', recipe.id]);
  }

  onAuthorClickFromTable(event: Event, authorName: string, clickedRecipeId: string) {
    event.stopPropagation();
    this.router.navigate(['/author', authorName, 'id', clickedRecipeId]);
  }

  onAuthorClickFromStats(authorName: string){
    this.router.navigate(['/author', authorName]);
  }

  onIngredientsChange(event: any) {
    const { originalEvent, value } = event
    if (value) {
      this.selectAll = value.length === this.ingredients.length;
      let ingredients = this.selectedIngredients.map(i => i.name);
      this.recipeParams.selectedIngredients = ingredients.join(',');
    }
    else {
      this.recipeParams.selectedIngredients = undefined;
    }
  }

  onIngredientsSearch($event: ListboxFilterEvent) {
    if ($event.originalEvent.target) {
      this.ingredientSearchTerm.next(($event.originalEvent.target as HTMLInputElement).value);
    }
  }

  onRecipeSearchChange(event: Event) {
    const target = event.target as HTMLInputElement;
    this.recipeParams.recipeName = target.value;
    this.recipeSearchTerm.next(target.value);
  }

  onSort(event: SortEvent) {
    console.log(event);
    if (event) {
      const sortField = event.field;
      const sortOrder = event.order === 1 ? 'asc' : 'desc';
      const formattedSortOrder = `${sortField}_${sortOrder}`;
      this.selectedSortOption = formattedSortOrder;
      this.getRecipes();
    }
  }

  onClearFilters() {
    this.selectedIngredients = [];
    this.selectAll = false;
    this.getRecipes();
  }

  onFilterRecipes() {
    this.getRecipes();
  }

  ngOnDestroy(): void {
    this.recipesSubscription.unsubscribe();
    this.ingredientsSubscription.unsubscribe();
    this.mostProlificAuthorsSubscription.unsubscribe();
    this.mostComplexRecipesSubscription.unsubscribe();
    this.mostUsedIngredientsSubscription.unsubscribe();
  }
}