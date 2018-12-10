IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BhartiAxa_InsertLeads_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BhartiAxa_InsertLeads_16]
GO

	
-- =============================================
-- Author:		Piyush	
-- Create datetime: 4/6/2016 10:50:58 AM
-- Description:	Added cityid in BhartiAxa_Leads
-- =============================================
CREATE PROCEDURE [dbo].[BhartiAxa_InsertLeads_16.4.2]

	 @MakeName VARCHAR(300) = NULL
	,@ModelName VARCHAR(300) = NULL
	,@VersionName VARCHAR(300) = NULL
	,@CityId INT
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
		,CityId
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
		,@CityId
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



