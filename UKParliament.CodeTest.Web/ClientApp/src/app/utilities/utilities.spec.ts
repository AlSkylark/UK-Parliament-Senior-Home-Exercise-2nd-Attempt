import { EmployeeViewModel } from "../models/employee-view-model";
import { Utilities } from "./utilities";

describe('Utilities', () => {
    it("Should return today's day when you pass nothing", () => {
        const result = Utilities.DateOnly();

        const today = new Date();
        const [year, month, day] = [
            today.getFullYear(),
            (today.getMonth() + 1).toString().padStart(2, "0"),
            today.getDate().toString().padStart(2, "0")
        ];

        expect(result).toBe(`${year}-${month}-${day}`);
    });

    [
        new Date("2025-03-12T15:30:00Z"),
        new Date("2000-01-01T00:00:00Z"),
        new Date("1999-12-31T23:59:59Z"),
        new Date("2010-07-04T12:00:00Z")
    ].forEach(testDate => {
        it(`Should sanitise the date for ${testDate.toISOString()}`, () => {
            const result = Utilities.DateOnly(testDate.toISOString());

            const [year, month, day] = [
                testDate.getFullYear(),
                (testDate.getMonth() + 1).toString().padStart(2, "0"),
                testDate.getDate().toString().padStart(2, "0")
            ];

            expect(result).toBe(`${year}-${month}-${day}`);
        })
    });

    it("Should clean empty strings", () => {
        const testObject = {
            name: "Some name",
            surname: ""
        }

        const result = Utilities.CleanEmptyObjects(testObject);

        expect(result).toEqual({ name: "Some name" });
    });

    it("Should clean null value", () => {
        const testObject = {
            name: "Some name",
            surname: null
        }

        const result = Utilities.CleanEmptyObjects(testObject);

        expect(result).toEqual({ name: "Some name" });
    });

    it("Should clean null array", () => {
        const testObject = {
            name: "Some name",
            array: []
        }

        const result = Utilities.CleanEmptyObjects(testObject);

        expect(result).toEqual({ name: "Some name" });
    });

    it("Should clean empty object", () => {
        const testObject = {
            name: "Some name",
            object: {}
        }

        const result = Utilities.CleanEmptyObjects(testObject);

        expect(result).toEqual({ name: "Some name" });
    });

    it("Should clean nested empty objects", () => {
        const testObject = {
            name: "Some name",
            object: {
                test: "all good",
                nestedObject: {}
            }
        }

        const result = Utilities.CleanEmptyObjects(testObject);

        expect(result).toEqual({ name: "Some name", object: { test: "all good" } });
    });

    it("Should clean nested empty arrays", () => {
        const testObject = {
            name: "Some name",
            object: {
                test: "all good",
                nestedArray: []
            }
        }

        const result = Utilities.CleanEmptyObjects(testObject);

        expect(result).toEqual({ name: "Some name", object: { test: "all good" } });
    });

    it("Should clean nested empty strings", () => {
        const testObject = {
            name: "Some name",
            object: {
                test: "all good",
                nestedString: ""
            }
        }

        const result = Utilities.CleanEmptyObjects(testObject);

        expect(result).toEqual({ name: "Some name", object: { test: "all good" } });
    });

    it("Should clean null strings", () => {
        const testObject: EmployeeViewModel = {
            department: "null",
            payBand: "null",
            manager: {},
            address: {},
            firstName: "Alex"
        };

        const expected: EmployeeViewModel = {
            department: "",
            payBand: "",
            manager: {},
            address: {},
            firstName: "Alex"
        };

        Utilities.CleanNullString(testObject);

        expect(testObject).toEqual(expected as any);
    })
});