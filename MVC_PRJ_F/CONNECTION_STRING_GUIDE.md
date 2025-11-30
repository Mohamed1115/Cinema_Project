# دليل ربط قاعدة البيانات بـ SQL Server

## خيارات Connection String المختلفة:

### 1. SQL Server LocalDB (للاختبار المحلي)
```
Server=(localdb)\\mssqllocaldb;Database=CinemaDB;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True
```

### 2. SQL Server على نفس الجهاز (Windows Authentication)
```
Server=localhost;Database=CinemaDB;Integrated Security=True;TrustServerCertificate=True;MultipleActiveResultSets=True
```

### 3. SQL Server على نفس الجهاز (SQL Authentication)
```
Server=localhost;Database=CinemaDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;MultipleActiveResultSets=True
```

### 4. SQL Server على خادم بعيد
```
Server=YourServerName;Database=CinemaDB;User Id=YourUsername;Password=YourPassword;TrustServerCertificate=True;MultipleActiveResultSets=True
```

### 5. SQL Server مع Instance محدد (مثل SQLEXPRESS)
```
Server=localhost\\SQLEXPRESS;Database=CinemaDB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;MultipleActiveResultSets=True
```

### 6. SQL Server مع Port محدد
```
Server=YourServerName,1433;Database=CinemaDB;User Id=YourUsername;Password=YourPassword;TrustServerCertificate=True;MultipleActiveResultSets=True
```

## خطوات الربط:

1. افتح ملف `appsettings.json`
2. استبدل قيمة `DefaultConnection` بأحد الخيارات أعلاه
3. استبدل:
   - `YourServerName` باسم السيرفر
   - `CinemaDB` باسم قاعدة البيانات
   - `YourUsername` و `YourPassword` ببيانات الدخول
4. احفظ الملف
5. شغل الأمر: `dotnet ef database update`

## ملاحظات مهمة:

- تأكد من أن SQL Server يعمل
- تأكد من أن قاعدة البيانات موجودة (أو اتركها فارغة لإنشائها تلقائياً)
- `TrustServerCertificate=True` مهم للاتصال الآمن
- `MultipleActiveResultSets=True` يسمح بتنفيذ عدة استعلامات في نفس الوقت

