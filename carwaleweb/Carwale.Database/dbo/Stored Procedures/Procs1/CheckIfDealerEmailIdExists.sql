IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckIfDealerEmailIdExists]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckIfDealerEmailIdExists]
GO

	-- Created by: Shikhar on 28-01-2015 
-- To check if the Dealer Email Id Exists
CREATE PROCEDURE [dbo].[CheckIfDealerEmailIdExists]
	@EmailId	VARCHAR(50) = NULL
AS

BEGIN
	SELECT DS.ID  FROM Dealers DS WITH (NOLOCK)  
	WHERE DS.EmailId = @EmailId
	UNION
	SELECT TD.ID FROM TempDealers TD WITH(NOLOCK) 
	WHERE TD.EmailId = @EmailId
END