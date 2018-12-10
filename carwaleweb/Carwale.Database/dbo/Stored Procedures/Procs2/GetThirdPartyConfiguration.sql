IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetThirdPartyConfiguration]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetThirdPartyConfiguration]
GO

	-- =============================================
-- Author:		Vinayak
-- Create date: 1/03/2016
-- Description:	Gets Third party configurations
-- Modified by Shalini Nair on 02/03/2016 to retrieve HttpRequestType and HttpRequestMessage
-- =============================================
create PROCEDURE [dbo].[GetThirdPartyConfiguration]
	-- Add the parameters for the stored procedure here
	@ThirdPartyLeadId INT
	,@ApiUrl VARCHAR(500) OUTPUT
	,@HttpRequestType TINYINT OUTPUT
	,@HttpRequestMessage VARCHAR(MAX) OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT @ApiUrl = Url
		,@HttpRequestType = tpSet.HttpRequestType
		,@HttpRequestMessage = HttpRequestMessage
		FROM ThirdPartyLeadSettings tpSet WITH (NOLOCK)
	INNER JOIN HTTPRequestTypes reqType WITH (NOLOCK) ON tpSet.HttpRequestType = reqType.Id
	WHERE ThirdPartyLeadSettingId=@ThirdPartyLeadId

END

