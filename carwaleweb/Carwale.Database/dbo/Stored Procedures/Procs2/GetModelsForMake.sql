IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelsForMake]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelsForMake]
GO

	-- =============================================
-- Author:		Akansha
-- Create date: 22-5-2013
-- Description:	Gets models for particular make
-- =============================================
CREATE PROCEDURE [dbo].[GetModelsForMake] 
@MakeId int
AS
BEGIN
	SELECT DISTINCT Lv.ModelId, Lv.MakeName, MO.Name as ModelName
	FROM LiveListings Lv WITH(NOLOCK) 
	INNER JOIN CarModels Mo ON MO.ID = Lv.ModelId
	Where Lv.MakeId = @MakeId
	ORDER BY ModelId
END
/****** Object:  StoredProcedure [dbo].[GetMaskingNumberForDealers]    Script Date: 05/23/2013 09:52:19 ******/
SET ANSI_NULLS ON
