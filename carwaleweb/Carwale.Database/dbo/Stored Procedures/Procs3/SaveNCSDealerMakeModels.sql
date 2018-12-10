IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveNCSDealerMakeModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveNCSDealerMakeModels]
GO

	-- =============================================
-- Author:		Chetan Thambad
-- Create date: 25/08/2016
-- Description:	SaveNCDDealerMakeModels 7007,'18','552,113,934,943,238,857,996,905,952,885'
-- =============================================
CREATE PROCEDURE [dbo].[SaveNCSDealerMakeModels]

	@dealerId int,
	@makeId VARCHAR(MAX),
	@modelId VARCHAR(MAX)
	
AS
BEGIN
	DELETE FROM NCS_DealerMakes WHERE dealerId = @dealerId;

	DELETE FROM TC_NoDealerModels WHERE dealerId = @dealerId;

	INSERT INTO NCS_DealerMakes (DealerId, MakeId)
    SELECT @dealerId, ListMember FROM [dbo].[fnSplitCSV] (@makeId)  

	INSERT INTO TC_NoDealerModels(DealerId, [Source], ModelId) 
	SELECT @dealerId, 2, ListMember FROM [dbo].[fnSplitCSV] (@modelId) -- source = 2 because it is NCD dealer
END

