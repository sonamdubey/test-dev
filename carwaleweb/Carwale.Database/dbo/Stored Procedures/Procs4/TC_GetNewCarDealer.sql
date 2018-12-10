IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetNewCarDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetNewCarDealer]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 26th Sep 2016
-- Description : To fetch new car dealers on the basis of tc_dealertypeId
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetNewCarDealer] 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id,(CONVERT(VARCHAR,Id) + ' - ' + Organization) AS Organization 
	FROM Dealers WITH(NOLOCK)
	WHERE IsTCDealer = 1 AND TC_DealerTypeId = 2
	ORDER BY ID
END
