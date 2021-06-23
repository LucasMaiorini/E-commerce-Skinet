import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.scss']
})
export class ServerErrorComponent implements OnInit {
  error: any;
  constructor(private router: Router) {
    // To handle the navigationExtras is necessary to treat it in the constructor.
    const navigation = this.router.getCurrentNavigation();
    // Checks if none of these steps has some error non related to the error.
    // Only if is allright with navigation, navigation.extras and navigation.extras.state the value of error is setted to error.
    this.error = navigation?.extras?.state?.error;
  }

  ngOnInit(): void {
  }

}
