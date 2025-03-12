import { TestBed } from "@angular/core/testing";
import { EmployeeService } from "./employee.service";
import { provideHttpClient } from "@angular/common/http";
import { HttpTestingController, provideHttpClientTesting } from "@angular/common/http/testing";
import { ResourceCollection } from "../models/resource-collection";
import { Resource } from "../models/resource";
import { EmployeeViewModel } from "../models/employee-view-model";

describe('EmployeeService', () => {
  let service: EmployeeService;
  let httpMock: HttpTestingController;
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [provideHttpClient(), provideHttpClientTesting(), { provide: 'BASE_URL', useValue: 'http://localhost/' }]
    });
    service = TestBed.inject(EmployeeService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('This is an example test for the EmployeeService', () => {
    expect(service).toBeTruthy();
  });

  it("Active employee should start at null", () => {
    expect(service.activeEmployee).toBe(null);
  });

  it('Should call the correct URL', () => {
    service.fetchEmployees();

    const req = httpMock.expectOne('http://localhost/api/employee?textSearch=&employeeType=&payBand=&department=&limit=21&page=1');
    expect(req.request.method).toBe('GET');
    req.flush(fakeData);

    expect(service.createEmployeeLink).toBe("test.com");
  });

  //Just a couple of examples of unit tests you can do with Karma
});

const fakeData: Resource<ResourceCollection<Resource<EmployeeViewModel>>> = {
  data: {
    pagination: { currentPage: 1, finalPage: 1, from: 1, to: 1, total: 1, perPage: 10 },
    results: [{
      data: {
        firstName: "Test guy",
        manager: {},
        address: {}
      },
      links: []
    }]
  },
  links: [{
    actions: ["GET"],
    href: "test.com",
    rel: "self"
  }]
}
