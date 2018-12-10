IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[OLM_OutletModelMapingSave]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[OLM_OutletModelMapingSave]
GO

	CREATE PROCEDURE [dbo].[OLM_OutletModelMapingSave]
--Name of SP/Function                       : CarWale.OLM_OutletModelMapingSave
--Applications using SP                     : CRM
--Modules using the SP                      : OLMOutletModelBinding.cs
--Technical department                      : Database
--Summary                                   : Saves modelId and dealeroutlet id
--Author                                    : AMIT Kumar 30-sept-2013
--Modification history                      : 


@outletId		INT,
@modelId		INT,
@IsActive		BIT,
@updateBy		BIGINT

AS 
BEGIN
	
	UPDATE OLM_OutletModelMapping SET IsActive = 1,UpdatedBy =@updateBy  WHERE ShowroomId = @outletId AND ModelId = @modelId
	IF(@@RowCount=0)
	BEGIN
		INSERT INTO OLM_OutletModelMapping(ShowroomId, ModelId,IsActive,UpdatedBy) 
		VALUES(@outletId, @modelId,@IsActive,@updateBy)
	END

END

