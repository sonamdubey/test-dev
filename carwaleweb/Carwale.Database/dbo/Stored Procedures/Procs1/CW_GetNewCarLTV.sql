IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_GetNewCarLTV]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_GetNewCarLTV]
GO

	CREATE PROCEDURE [dbo].[CW_GetNewCarLTV]
@ModelId INT
AS
--Author:Rakesh Yadav On 1 Aug 2015
-- Desc: Fetch New car LTV for model
BEGIN
	SELECT DISTINCT [3] AS LTVUpTo36,[5] AS LTV36To60,[7] AS LTVGT60,IsActive,CarModelId AS ModelId,MakeId AS CarMakeId,CarName FROM
	(SELECT NCL.LTV,NCL.Tenor,NCL.IsActive,NCL.CarModelId,VM.MakeId,VM.Make+' '+VM.Model AS CarName FROM CW_NewCarLTV NCL
	JOIN vwMMV VM ON NCL.CarModelId=VM.ModelId
	 WHERE NCL.CarModelId=@ModelId ) A PIVOT (MAX(LTV) FOR Tenor IN ([3],[5],[7])) B
END

