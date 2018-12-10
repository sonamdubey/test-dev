IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetNCSDealerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetNCSDealerDetails]
GO

	-- =============================================
-- Author: Vinayak
-- Create date: 03/11/2015
-- Description:	Get NCS Dealers details on dealer id
-- exec [dbo].[GetNCSDealerDetails] 10
-- =============================================
CREATE PROCEDURE [dbo].[GetNCSDealerDetails]
	-- Add the parameters for the stored procedure here
	@DealerId INT
AS
BEGIN
	SELECT NCSD.DealerTitle + ISNULL(NCSD.AreaName + ' - ', '') + ISNULL(' , ' + DealerCode , '') AS DealerName
	FROM NCS_Dealers NCSD WITH (NOLOCK)
	WHERE NCSD.ID=@DealerId
	END