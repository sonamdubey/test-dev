IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetModelStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetModelStatus]
GO

	
--Author: Ravi Koshal  
--Date:16 July 2013  
--Desc: Get ModelStatus from modelId
CREATE PROCEDURE [cw].[GetModelStatus]   -- Exec cw.GetMakeIdFromMakeName 'marutisuzuki'

@ModelId INT

AS  

BEGIN  


SELECT New FROM CarModels WITH (NOLOCK)
where ID = @ModelId and IsDeleted = 0 


END
