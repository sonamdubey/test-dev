IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_InsertCustomer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_InsertCustomer]
GO

	
CREATE Proc [dbo].[NCD_InsertCustomer]
(
@Name Varchar(50),
@Email Varchar(80),
@Mobile Varchar(10),
@CustomerSource tinyint
)
as
if not exists (select * from ncd_customers where Email=@Email)
	begin
		insert into ncd_customers (Name,email,Mobile,EntryDate,CustomerSource,IsActive)
		values(@Name,@Email,@Mobile,GETDATE(),@CustomerSource,1)
		select @@IDENTITY 'ID'
	end
else
	begin
		select ID from ncd_customers where Email=@Email
	end

