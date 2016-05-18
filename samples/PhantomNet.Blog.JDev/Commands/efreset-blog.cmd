dnx -p .. ef database update 0 -c BlogDbContext
dnx -p .. ef migrations remove -c BlogDbContext
dnx -p .. ef migrations add Initial -c BlogDbContext
dnx -p .. ef database update -c BlogDbContext
