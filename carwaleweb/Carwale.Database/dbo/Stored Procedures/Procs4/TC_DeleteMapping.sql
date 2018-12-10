IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DeleteMapping]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DeleteMapping]
GO

	-- =============================================
-- Author	:	Ajay Singh(2 feb 2016)
-- Description	:	To Delete  mapping of multioutlet/group
-- Modifier : Amit Yadav(15 Feb 2016)
-- Purpose	: To insert the data before delete into log table.
-- =============================================
CREATE PROCEDURE [dbo].[TC_DeleteMapping]
@Id INT 
AS
BEGIN
	--Insert the data in TC_DealerMappingLog table before delete Amit Yadav(15-02-2016)
	INSERT INTO TC_DealerAdminMappingLog(TC_DealerAdminMappingId,DealerAdminId,DealerId,IsGroup,CreatedOn,DeletedOn)
	SELECT Id,DealerAdminId,DealerId,IsGroup,CreatedOn,GETDATE() FROM TC_DealerAdminMapping WITH(NOLOCK)
	WHERE Id = @Id
	--Delete the data
	DELETE FROM TC_DealerAdminMapping WHERE Id = @Id
END

------------------------------------------------------ Sunil Yadav : to update ContractStatus and ContracEnd Date   ----------------------------

SET QUOTED_IDENTIFIER ON
