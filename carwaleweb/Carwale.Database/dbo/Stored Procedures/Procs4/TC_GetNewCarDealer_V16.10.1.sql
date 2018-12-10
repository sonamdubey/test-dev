IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetNewCarDealer_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetNewCarDealer_V16]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 26th Sep 2016
-- Description : To fetch new car dealers on the basis of tc_dealertypeId
-- Modified by ruchira Patil on 10th Oct 2016 (concatenated - dealer name and then Id.)
-- =============================================
create PROCEDURE [dbo].[TC_GetNewCarDealer_V16.10.1] 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT Id,(Organization + ' - ' + CONVERT(VARCHAR,Id)) AS Organization 
	FROM Dealers D WITH(NOLOCK)
	WHERE IsTCDealer = 1 AND TC_DealerTypeId = 2
	ORDER BY D.Organization
END