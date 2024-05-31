import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ContactComponent } from './contact/contact.component';
import { AboutComponent } from './about/about.component';
import { TestErrorComponent } from './core/test-error/test-error.component';
import { NotFoundComponent } from './core/not-found/not-found.component';
import { ServerErrorComponent } from './core/server-error/server-error.component';
import { authGuard } from './core/guards/authGuard';

const routes: Routes = [
  {path: '', component: HomeComponent, data: {breadcrumb: 'Home'}},
  {path: 'test-error', component: TestErrorComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: 'shop', loadChildren: () => import('./shop/shop.module').then(m => m.ShopModule)},
  // {path: 'contact', component: ContactComponent},
  // {path: 'about', component: AboutComponent},
  {path: 'cart', loadChildren: () => import('./cart/cart.module').then(m => m.CartModule)},
  {path: 'checkout', canActivate: [authGuard], loadChildren: () => import('./checkout/checkout.module').then(m => m.CheckoutModule)},
  {path: 'account', loadChildren: () => import('./account/account.module').then(m => m.AccountModule),
  data: { breadcrumb: { skip: true } }
  },
  {path: 'orders', canActivate: [authGuard], loadChildren: () => import('./orders/orders.module').then(m => m.OrdersModule),
  data: { breadcrumb: 'Orders' }},
  {path: '**', redirectTo: '', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes, {anchorScrolling: 'enabled'})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
