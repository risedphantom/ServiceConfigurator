CREATE proc [dbo].[GetRestrictionsToUpdate]
as
begin	
	declare @Now datetime = getdate()

	select	IR.ID
		,	TypeName
		,	[Rule]
		,	cast(1 as bit) as Switch
		,	S.Hostname
		,	S.SiteName
	from	IISRestriction IR
			join IISSite S on IR.IISSiteID = S.ID
	where	(@Now between StartMoment and StopMoment)
			and [Enabled] = 0
			and Error is null

	union all

	select	IR.ID
		,	TypeName
		,	[Rule]
		,	cast(0 as bit) as Switch
		,	S.Hostname
		,	S.SiteName
	from	IISRestriction IR
			join IISSite S on IR.IISSiteID = S.ID
	where	(@Now not between StartMoment and StopMoment)
			and [Enabled] = 1
			and Error is null
end