IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_AddMPlanMaster]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_AddMPlanMaster]
GO

	
-- =============================================
-- Author:		<Deepak Tripathi>
-- Create date: <21-July-2008>
-- Description:	<Add SpareParts>
-- =============================================
CREATE PROCEDURE [dbo].[Con_AddMPlanMaster]
	-- Add the parameters for the stored procedure here
	@Id				NUMERIC,
	@PlanName		VARCHAR(100),
	@MakeId			NUMERIC,
	@Status			BIT OUTPUT
AS
BEGIN
	IF @Id = -1
		BEGIN
			SELECT Id FROM Con_MaintenancePlanMaster 
			WHERE PlanName = @PlanName AND MakeId = @MakeId

			IF @@RowCount = 0
				BEGIN
					INSERT INTO Con_MaintenancePlanMaster ( PlanName, MakeId )
					VALUES ( @PlanName, @MakeId )
					
					SET @Status = 1
				END
		END
	ELSE
		BEGIN
			SELECT Id FROM Con_MaintenancePlanMaster 
			WHERE PlanName = @PlanName AND MakeId = @MakeId AND Id <> @Id 

			IF @@RowCount = 0
				BEGIN
					UPDATE Con_MaintenancePlanMaster 
					SET PlanName = @PlanName, MakeId = @MakeId  WHERE Id = @Id
					SET @Status = 1
				END
		END
END

