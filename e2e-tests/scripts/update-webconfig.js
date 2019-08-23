/*
 * Copyright (c) 2019, Okta, Inc. and/or its affiliates. All rights reserved.
 * The Okta software accompanied by this notice is provided pursuant to the Apache License, Version 2.0 (the "License.")
 *
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *
 * See the License for the specific language governing permissions and limitations under the License.
 */
'use strict';

const fs = require('fs');
const { execSync } = require('child_process');
const path = require('path');
const url = require('url');

// Users can also provide the testenv configuration at the root folder
require('dotenv').config({ path: path.join(__dirname, '..', 'testenv') });

function updateConfig(file) {
  if (!process.env.ISSUER || !process.env.CLIENT_ID || !process.env.CLIENT_SECRET || !process.env.USER_NAME || !process.env.PASSWORD) {
    console.log('[ERROR] Please set the necessary Environment variables (ISSUER, CLIENT_ID, CLIENT_SECRET, USER_NAME, PASSWORD)');
    process.exit(1);
  }

  const urlProperties = url.parse(process.env.ISSUER);
  const OKTA_DOMAIN = urlProperties.host;

  const data = fs.readFileSync(file, 'utf8');
  let result = data.replace(/{ClientId}/g, process.env.CLIENT_ID);
  result = result.replace(/{ClientSecret}/g, process.env.CLIENT_SECRET);
  result = result.replace(/{yourOktaDomain}/g, OKTA_DOMAIN);
  fs.writeFileSync(file, result, 'utf8');
}

function cloneRepository(repository, directory) {
  const dir = path.join(__dirname, '..', directory);
  if (fs.existsSync(dir)) {
    console.log(`${directory} is already cloned. Getting latest version...`);
    execSync(`cd ${directory} && git pull`)
    return;
  }

  const command = `git clone ${repository}`;
  console.log(`Cloning repository ${directory}`);
  execSync(command);
}

// Update the Web.config in the dist directory with your org & webapp details
updateConfig(path.join(__dirname, '../..', 'okta-hosted-login', '/dist/okta-aspnetcore-mvc-example/appsettings.json'));
updateConfig(path.join(__dirname, '../..', 'self-hosted-login', '/dist/okta-aspnetcore-mvc-example/appsettings.json'));
cloneRepository('https://github.com/okta/okta-oidc-tck.git', 'okta-oidc-tck');
