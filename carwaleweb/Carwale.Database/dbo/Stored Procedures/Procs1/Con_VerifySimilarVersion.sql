IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_VerifySimilarVersion]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_VerifySimilarVersion]
GO

	-- =============================================
-- Author:		<Vinay Kumar Prajapati>
-- Create date: < 04th April 2014>
-- Description:	<Check similar version from carModels and save related data>
-- =============================================
CREATE PROCEDURE [dbo].[Con_VerifySimilarVersion](
 @Id INT,
 @Status BIT OUTPUT
)
AS

BEGIN 
    
	SELECT CM.Id FROM CarModels AS CM WITH(NOLOCK) WHERE CM.CarVersionID_Top = @Id
	  	
	IF @@ROWCOUNT <> 0
		BEGIN
		    --run sp and save similar version data
			EXEC Con_AP_GetModelTopVersions
			SET @Status=1
		END
	ELSE
		SET @Status=0

END
