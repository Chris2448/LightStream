﻿<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="ProjectWorker" generation="1" functional="0" release="0" Id="43465f66-d25a-4205-86f0-e5fcbe4d7255" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="ProjectWorkerGroup" generation="1" functional="0" release="0">
      <settings>
        <aCS name="WorkerRole:dbconnectionstring" defaultValue="">
          <maps>
            <mapMoniker name="/ProjectWorker/ProjectWorkerGroup/MapWorkerRole:dbconnectionstring" />
          </maps>
        </aCS>
        <aCS name="WorkerRole:dbname" defaultValue="">
          <maps>
            <mapMoniker name="/ProjectWorker/ProjectWorkerGroup/MapWorkerRole:dbname" />
          </maps>
        </aCS>
        <aCS name="WorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ProjectWorker/ProjectWorkerGroup/MapWorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="WorkerRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ProjectWorker/ProjectWorkerGroup/MapWorkerRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <maps>
        <map name="MapWorkerRole:dbconnectionstring" kind="Identity">
          <setting>
            <aCSMoniker name="/ProjectWorker/ProjectWorkerGroup/WorkerRole/dbconnectionstring" />
          </setting>
        </map>
        <map name="MapWorkerRole:dbname" kind="Identity">
          <setting>
            <aCSMoniker name="/ProjectWorker/ProjectWorkerGroup/WorkerRole/dbname" />
          </setting>
        </map>
        <map name="MapWorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ProjectWorker/ProjectWorkerGroup/WorkerRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapWorkerRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ProjectWorker/ProjectWorkerGroup/WorkerRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="WorkerRole" generation="1" functional="0" release="0" software="C:\Users\Matin Mansouri\Documents\Visual Studio 2015\Projects\ProjectWorker\ProjectWorker\csx\Debug\roles\WorkerRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="dbconnectionstring" defaultValue="" />
              <aCS name="dbname" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;WorkerRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;WorkerRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ProjectWorker/ProjectWorkerGroup/WorkerRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/ProjectWorker/ProjectWorkerGroup/WorkerRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/ProjectWorker/ProjectWorkerGroup/WorkerRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="WorkerRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="WorkerRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="WorkerRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
</serviceModel>