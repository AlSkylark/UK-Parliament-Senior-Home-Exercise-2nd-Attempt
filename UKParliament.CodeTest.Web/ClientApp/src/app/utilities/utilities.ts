export class Utilities {
    public static DateOnly(date?: string) {
        if (!date) {
            return new Date().toISOString().split("T")[0];
        }

        return new Date(date!).toISOString().split("T")[0];
    }

    public static CleanEmptyObjects(obj: Object): any {
        let clean: any = {}
        for (const entry in obj) {
            let val = obj[entry as keyof Object];
            if (val instanceof Object) {
                const recursed = this.CleanEmptyObjects(val);
                if (Object.keys(recursed).length === 0) {
                    continue;
                }
            }

            if (val) {
                clean[entry] = val;
            }
        }
        return clean;
    }
}