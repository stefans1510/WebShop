import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';

@Component({
  selector: 'app-checkout-address',
  templateUrl: './checkout-address.component.html',
  styleUrls: ['./checkout-address.component.scss']
})
export class CheckoutAddressComponent {
  @Input() checkoutForm?: FormGroup;

  constructor(private accountService: AccountService, private toastr: ToastrService) {}

  saveUserAddress() {
    var formData = this.checkoutForm?.get('addressForm')?.value;

    this.accountService.updateUserAddress(formData).subscribe({
      next: () => {
        this.toastr.success('Address saved!');
        this.checkoutForm?.get('addressForm')?.reset(formData);
      }
    })
  }
}
