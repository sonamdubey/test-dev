IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BhartiAxa_InsertLeads]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BhartiAxa_InsertLeads]
GO

	
-- =============================================
-- Author:		Akansha	
-- Create date: 28.02.2014	
-- Description:	Insert Bharti Axa Leads
-- =============================================
CREATE PROCEDURE [dbo].[BhartiAxa_InsertLeads] @MakeName VARCHAR(300)
	
	,@ModelName VARCHAR(300)
	,@VersionName VARCHAR(300)
	,@VersionId INT
	,@CityName VARCHAR(300)
	,@CustName VARCHAR(300)
	,@CustMobile VARCHAR(15)
	,@CustEmail VARCHAR(300)
	,@RegistrationDate DATE
	,@RequestDateTime DATETIME
	,@State varchar(300)
	,@ExpiryDate DATE
	,@InsType BIT
	,@NCB INT
	,@Zone VARCHAR(50)
	,@Insurer VARCHAR(200)
	,@BhartiAxaQuoteID VARCHAR(50)
	,@RemoteAddr VARCHAR(60)
	,@ClientIp VARCHAR(60)
	,@RecordId INT = 0 OUTPUT
	
AS
BEGIN
	INSERT INTO BhartiAxa_Leads (
		MakeName
		,ModelName
		,VersionName
		,VersionId
		,CityName
		,CustName
		,CustMobile
		,CustEmail
		,RegistrationDate
		,RequestDateTime
		,State
		,ExpiryDate
		,InsType
		,NCB
		,Zone
		,Insurer
		,BhartiAxaQuoteID
		,RemoteAddr
		,ClientIp
		)
	VALUES (
		@MakeName
		,@ModelName
		,@VersionName
		,@VersionId
		,@CityName
		,@CustName
		,@CustMobile
		,@CustEmail
		,@RegistrationDate
		,@RequestDateTime
		,@State
		,@ExpiryDate
		,@InsType
		,@NCB
		,@Zone
		,@Insurer
		,@BhartiAxaQuoteID
		,@RemoteAddr
		,@ClientIp
		)

	SET @RecordId = SCOPE_IDENTITY()
END



