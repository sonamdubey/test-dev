IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckCarIsBookMarked]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckCarIsBookMarked]
GO

	
-- =============================================
-- Author:		Akansha Srivastava
-- Create date: 24.04.2014
-- Description:	Check if a used car is bookmarked by the customer
-- =============================================
CREATE PROCEDURE [dbo].[CheckCarIsBookMarked]  --3018,'D9020',''
@CustomerId BIGINT
	,@ProfileId VARCHAR(50)
	,@IsBookMarked TINYINT Output
AS
BEGIN
	SELECT @IsBookMarked = count(id)
	FROM CustomerFavouritesUsed with (nolock)
	WHERE CustomerId = @CustomerId
		AND CarProfileId = @ProfileId
END

