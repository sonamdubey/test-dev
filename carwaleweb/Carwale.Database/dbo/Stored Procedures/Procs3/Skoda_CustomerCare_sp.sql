IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Skoda_CustomerCare_sp]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Skoda_CustomerCare_sp]
GO

	
create proc [dbo].[Skoda_CustomerCare_sp]
@Name Varchar(50),
@EmailId Varchar(100),
@Tele_MobileNo Varchar(10),
@City Int,
@Model Varchar(20),
@ChasisNo Varchar(50),
@RegistrationNo Varchar(50),
@DateofPurchase date,
@KMsRun Numeric,
@SellingDealer Varchar(50),
@ServiceDealer Varchar(50),
@Concern Varchar(250)
As
Begin 
Insert into Skoda_CustomerCare(Name, EmailId, [Tele/Mobile], City, Model, ChasisNo, RegistrationNo, Dateofpurchase, KMsRun, SellingDealer, ServiceDealer, Concern, EntryDate)
values
(@Name, @EmailId, @Tele_MobileNo, @City, @Model, @ChasisNo, @RegistrationNo, @DateofPurchase, @KMsRun, @SellingDealer, @ServiceDealer, @Concern,GETDATE())
End

