IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertThirdPartyLeadQueue_16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertThirdPartyLeadQueue_16]
GO

	-- =============================================
-- Author:		Sanjay Soni
-- Create date: 1/03/2016
-- Description:	Inserts a record into ThirdPartyLeadQueue
-- Modified by Shalini Nair on 02/03/2016 to retrieve HttpRequestType and HttpRequestMessage
-- =============================================
CREATE PROCEDURE [dbo].[InsertThirdPartyLeadQueue_16.3.1]
	-- Add the parameters for the stored procedure here
	@PQId NUMERIC(18, 0)
	,@ModelName VARCHAR(30)
	,@ModelId INT
	,@CityId INT
	,@CustomerName VARCHAR(100)
	,@Email VARCHAR(100)
	,@Mobile VARCHAR(20)
	,@OUTCity VARCHAR(30) OUTPUT
	,@ThirdPartyLeadId NUMERIC(18, 0) OUTPUT
	,@ApiUrl VARCHAR(500) OUTPUT
	,@HttpRequestType TINYINT OUTPUT
	,@HttpRequestMessage VARCHAR(MAX) OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @MakeId NUMERIC
		,@City VARCHAR(30)

	SELECT @MakeId = CM.CarMakeId
	FROM CarModels CM WITH (NOLOCK)
	WHERE CM.ID = @ModelId

	IF (
			@City = ''
			OR @City IS NULL
			)
		SELECT @City = NAME
		FROM Cities WITH (NOLOCK)
		WHERE ID = @CityId

	SELECT @ThirdPartyLeadId = ThirdPartyLeadSettingId
		,@ApiUrl = Url
		,@HttpRequestType = tpSet.HttpRequestType
		,@HttpRequestMessage = HttpRequestMessage
		,@OUTCity = @City
	FROM ThirdPartyLeadSettings tpSet WITH (NOLOCK)
	INNER JOIN HTTPRequestTypes reqType WITH (NOLOCK) ON tpSet.HttpRequestType = reqType.Id
	WHERE (
			ModelId = @ModelId
			OR (
				tpSet.ModelId = - 1
				AND MakeId = (
					SELECT CarMakeId
					FROM CarModels WITH (NOLOCK)
					WHERE ID = @ModelId
					)
				)
			)
		AND IsActive = 1

	INSERT INTO ThirdPartyLeadQueue (
		PQId
		,ModelName
		,MakeId
		,ModelId
		,TPLeadSettingId
		,CityId
		,City
		,CustomerName
		,Email
		,Mobile
		,PushStatus
		)
	VALUES (
		@PQId
		,@ModelName
		,@MakeId
		,@ModelId
		,@ThirdPartyLeadId
		,@CityId
		,@City
		,@CustomerName
		,@Email
		,@Mobile
		,- 1
		)

	SET @ThirdPartyLeadId = SCOPE_IDENTITY()
END