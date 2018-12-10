IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_MultiOutletDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_MultiOutletDealer]
GO

	
-- =============================================
-- Author:		<vivek rajak>
-- Create date: <08/05/15>
-- Description:	<To select state and cities on parameter DealerId>
-- =============================================
CREATE PROCEDURE [dbo].[TC_MultiOutletDealer]
	@DealerId varchar(20)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DLR.CityId AS CityId, C.Name AS City, C.StateId AS StateId,
         S.Name AS State 
	FROM Dealers AS DLR
		   
		   INNER JOIN Cities AS C ON C.ID = DLR.CityId 
		   INNER JOIN States AS S ON S.ID = C.StateId 
	WHERE DLR.ID = @DealerId
END

