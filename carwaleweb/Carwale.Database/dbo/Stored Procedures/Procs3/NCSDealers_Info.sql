IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCSDealers_Info]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCSDealers_Info]
GO

	
-- =============================================
-- Author:		sanjay soni
-- Create date: 07/03/2016
-- Description:	Return Dealer Name based on NCS Dealer Id. 
-- =============================================
CREATE PROCEDURE [dbo].[NCSDealers_Info] @DealerID INT
AS
BEGIN
	SELECT NCSD.DealerTitle AS DealerName
	FROM NCS_Dealers NCSD WITH(NOLOCK)
	WHERE NCSD.ID = @DealerID
END
