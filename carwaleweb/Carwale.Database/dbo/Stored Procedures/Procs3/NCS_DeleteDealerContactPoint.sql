IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCS_DeleteDealerContactPoint]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCS_DeleteDealerContactPoint]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 18th June 2014
-- Description:	Deleting the Contact Point and logging it
-- =============================================
CREATE PROCEDURE [dbo].[NCS_DeleteDealerContactPoint]
	@Ids		VARCHAR(MAX),
	@DeletedBy	INT,
	@Status		BIT OUTPUT 
AS
BEGIN
	INSERT INTO NCS_DealerContactPointLog (ContactPtId,DealerId,ModelId,MakeId,ContactName,Designation,Email,Mobile,AlternateMobile,ContactPointType,UpdatedBy,UpdatedOn) 
	SELECT Id,DealerId,ModelId,MakeId,ContactName,Designation,Email,Mobile,AlternateMobile,ContactPointType,@DeletedBy,GETDATE() FROM NCS_DealerContactPoint 
	WHERE ID IN (SELECT ListMember FROM fnSplitCSV(@Ids))

	DELETE FROM NCS_DealerContactPoint WHERE ID IN (SELECT ListMember FROM fnSplitCSV(@Ids))

	SET @Status = 1
END




