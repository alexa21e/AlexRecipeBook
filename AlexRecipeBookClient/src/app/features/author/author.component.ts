import { Component, OnInit } from '@angular/core';
import { Subject, Subscription, debounceTime, distinctUntilChanged, switchMap } from 'rxjs';
import { HomeRecipe } from '../../shared/models/homeRecipe';
import { Ingredient } from '../../shared/models/ingredient';
import { IngredientParams } from '../../shared/models/ingredientParams';
import { RecipesService } from '../../shared/services/recipes.service';
import { IngredientsService } from '../../shared/services/ingredients.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ListboxFilterEvent } from 'primeng/listbox';
import { SortEvent } from 'primeng/api';
import { AuthorRecipeParams } from '../../shared/models/authorRecipeParams';
import { RecipeName } from '../../shared/models/recipeName';

@Component({
  selector: 'app-author',
  templateUrl: './author.component.html',
  styleUrl: './author.component.css'
})
export class AuthorComponent implements OnInit {
  private recipesSubscription: Subscription = new Subscription();
  private ingredientsSubscription: Subscription = new Subscription();
  private clickedRecipeSubscription: Subscription = new Subscription();

  private ingredientSearchTerm = new Subject<string>();
  private recipeSearchTerm = new Subject<string>();
  searchText: string = '';

  recipes: HomeRecipe[] = [];
  ingredients: Ingredient[] = [];
  clickedRecipe?: RecipeName;

  recipeParams = new AuthorRecipeParams();
  ingredientsParams = new IngredientParams();

  recipesCount: number = 0;
  rowsPerPageOptions: number[] = [20];

  selectedIngredients!: any[];
  selectAll: any;

  authorName: string = '';
  clickedRecipeState: boolean = false;
  clickedRecipeId?: string;

  selectedSortOption: string = '_asc';

  isLoading = true;

  constructor(
    private recipesService: RecipesService,
    private ingredientsService: IngredientsService,
    private router: Router,
    private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    let authorName = this.route.snapshot.paramMap.get('name');
    if (authorName) this.authorName = authorName;

    let clickedRecipeId = this.route.snapshot.paramMap.get('id');
    if (clickedRecipeId) {
      this.clickedRecipeId = clickedRecipeId;
      this.clickedRecipeState = true;
      this.getClickedRecipe();
    }

    this.getRecipesByAuthor();
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
        return this.recipesService.getRecipesByAuthor(this.recipeParams);
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
  }

  getRecipesByAuthor() {
    this.recipeParams.sortOrder = this.selectedSortOption;
    this.recipeParams.authorName = this.authorName;
    if(this.clickedRecipeId){
      this.recipeParams.clickedRecipe = this.clickedRecipeState;
      this.recipeParams.clickedRecipeId = this.clickedRecipeId;
    }
    this.recipesSubscription = this.recipesService.getRecipesByAuthor(this.recipeParams).subscribe(
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

  getClickedRecipe() {
    if (this.clickedRecipeId) {
      this.clickedRecipeSubscription = this.recipesService.getRecipeNameById(this.clickedRecipeId).subscribe(
        {
          next: (response) => {
            this.clickedRecipe = response;
          },
          error: error => console.log(error)
        }
      );
    }
  }

  onPageChange(event: any) {
    const newPageNumber = event.first / event.rows + 1;
    if (this.recipeParams.pageNumber !== newPageNumber) {
      this.recipeParams.pageNumber = newPageNumber;
      this.getRecipesByAuthor();
    }
  }

  onRecipeClick(recipe: HomeRecipe) {
    this.router.navigate(['/recipe', recipe.id]);
  }

  onRecipeHover(id: string) {
    this.router.navigate(['/recipe', id]);
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

  onSort(event: SortEvent) {
    console.log(event);
    if (event) {
      const sortField = event.field;
      const sortOrder = event.order === 1 ? 'asc' : 'desc';
      const formattedSortOrder = `${sortField}_${sortOrder}`;
      this.selectedSortOption = formattedSortOrder;
      this.getRecipesByAuthor();
    }
  }

  onRecipeSearchChange(event: Event) {
    const target = event.target as HTMLInputElement;
    this.recipeParams.recipeName = target.value;
    this.recipeSearchTerm.next(target.value);
  }

  onClearFilters() {
    this.selectedIngredients = [];
    this.selectAll = false;
    this.getRecipesByAuthor();
  }

  onFilterRecipes() {
    this.getRecipesByAuthor();
  }

  ngOnDestroy(): void {
    this.recipesSubscription.unsubscribe();
    this.ingredientsSubscription.unsubscribe();
    this.clickedRecipeSubscription.unsubscribe();
  }
}
